const API_BASE = 'http://localhost:4000/api';
const tokenKey = 'token';

function getToken() { return localStorage.getItem(tokenKey); }
function setToken(t) { localStorage.setItem(tokenKey, t); }
function logout() { localStorage.removeItem(tokenKey); alert('Đã đăng xuất'); location.href = 'index.html'; }

async function api(path, options = {}) {
  const headers = { 'Content-Type': 'application/json', ...(options.headers || {}) };
  const token = getToken();
  if (token) headers['Authorization'] = `Bearer ${token}`;
  const res = await fetch(`${API_BASE}${path}`, { ...options, headers });
  let data;
  try { data = await res.json(); } catch { data = {}; }
  if (!res.ok) throw new Error(data.message || res.statusText);
  return data;
}

// Hiển thị thông tin người dùng
(function showUserInfo() {
  const emailEl = document.getElementById('userEmail') || document.getElementById('username');
  const roleEl = document.getElementById('userRole') || document.getElementById('role');
  const token = getToken();
  if (emailEl && roleEl) {
    try {
      if (token) {
        const payload = JSON.parse(atob(token.split('.')[1]));
        emailEl.textContent = payload.email || 'Ẩn';
        roleEl.textContent = payload.role || 'Ẩn';
      } else {
        emailEl.textContent = 'Chưa đăng nhập';
        roleEl.textContent = '---';
      }
    } catch {
      emailEl.textContent = 'Không xác định';
      roleEl.textContent = 'Không xác định';
    }
  }
})();

// Đăng ký
const registerForm = document.getElementById('registerForm');
if (registerForm) {
  registerForm.addEventListener('submit', async (e) => {
    e.preventDefault();
    const body = Object.fromEntries(new FormData(registerForm).entries());
    try {
      await api('/auth/register', { method: 'POST', body: JSON.stringify(body) });
      alert('Đăng ký thành công, hãy đăng nhập.');
      location.href = 'login.html';
    } catch (err) { alert(err.message); }
  });
}

// Đăng nhập
const loginForm = document.getElementById('loginForm');
if (loginForm) {
  loginForm.addEventListener('submit', async (e) => {
    e.preventDefault();
    const body = Object.fromEntries(new FormData(loginForm).entries());
    try {
      const data = await api('/auth/login', { method: 'POST', body: JSON.stringify(body) });
      setToken(data.token);
      alert('Đăng nhập thành công');
      location.href = 'menu.html';
    } catch (err) { alert(err.message); }
  });
}

// Menu & bộ lọc
const dishesEl = document.getElementById('dishes');
const searchInput = document.getElementById('searchInput');
const searchBtn = document.getElementById('searchBtn');
const filterCategory = document.getElementById('filterCategory');
const filterRating = document.getElementById('filterRating');
const filterPrice = document.getElementById('filterPrice');

function clearFilters() {
  if (searchInput) searchInput.value = '';
  if (filterCategory) filterCategory.value = '';
  if (filterRating) filterRating.value = '';
  if (filterPrice) filterPrice.value = '';
  loadDishes();
}
window.clearFilters = clearFilters;

async function loadDishes() {
  if (!dishesEl) return;
  try {
    const q = searchInput?.value?.trim();
    const list = await api(`/dishes${q ? `?q=${encodeURIComponent(q)}` : ''}`);
    let filtered = list;

    if (filterCategory?.value) filtered = filtered.filter(d => d.category === filterCategory.value);
    if (filterRating?.value) filtered = filtered.filter(d => (d.rating || 0) >= parseFloat(filterRating.value));
    if (filterPrice?.value === 'low') filtered = filtered.filter(d => d.price <= 50000);
    if (filterPrice?.value === 'high') filtered = filtered.filter(d => d.price > 50000);

    dishesEl.innerHTML = filtered.map(d => `
      <li class="card">
        <b>${d.name}</b><br/>
        ${d.price}đ · ★ ${d.rating?.toFixed ? d.rating.toFixed(1) : (d.rating ?? '4.0')} · <span class="badge">${d.category}</span><br/>
        <button onclick="addToCart('${d._id}', '${String(d.name).replace(/'/g, "\\'")}', ${d.price})">Thêm vào giỏ</button>

        <div class="review">
          <h4>Đánh giá món</h4>
          <select id="rating-${d._id}">
            <option value="5">5 sao</option>
            <option value="4">4 sao</option>
            <option value="3">3 sao</option>
            <option value="2">2 sao</option>
            <option value="1">1 sao</option>
          </select>
          <input id="comment-${d._id}" placeholder="Nhận xét..." />
          <button onclick="submitReview('${d._id}')">Gửi đánh giá</button>
        </div>
      </li>
    `).join('');
  } catch (err) { alert(err.message); }
}

if (searchBtn && dishesEl) {
  searchBtn.addEventListener('click', () => loadDishes());
  loadDishes();
}

// Reviews
async function submitReview(dishId) {
  const rating = Number(document.getElementById(`rating-${dishId}`).value);
  const comment = document.getElementById(`comment-${dishId}`).value;
  try {
    await api(`/dishes/${dishId}/reviews`, { method: 'POST', body: JSON.stringify({ rating, comment }) });
    alert('Đánh giá thành công!');
    loadDishes();
  } catch (err) { alert(err.message); }
}
window.submitReview = submitReview;

// Giỏ hàng
const cartItemsEl = document.getElementById('cartItems');
const cartTotalEl = document.getElementById('cartTotal');
let cart = [];

function addToCart(id, name, price) {
  const existing = cart.find(i => i.dishId === id);
  if (existing) existing.quantity += 1;
  else cart.push({ dishId: id, name, price, quantity: 1 });
  renderCart();
}
window.addToCart = addToCart;

function renderCart() {
  if (!cartItemsEl || !cartTotalEl) return;
  cartItemsEl.innerHTML = cart.map(i => `
    <li>${i.quantity} x ${i.name} - ${i.price}đ</li>
  `).join('');
  const total = cart.reduce((sum, i) => sum + i.price * i.quantity, 0);
  cartTotalEl.textContent = `${total}đ`;
}

async function placeOrder() {
  try {
    if (cart.length === 0) return alert('Giỏ hàng trống');
    const items = cart.map(i => ({ dishId: i.dishId, quantity: i.quantity }));
    const order = await api('/orders', { method: 'POST', body: JSON.stringify({ items }) });
    alert(`Đặt hàng thành công! Mã đơn: ${order.code || order._id}`);
    cart = [];
    renderCart();
  } catch (err) { alert(err.message); }
}
window.placeOrder = placeOrder;

// Đơn hàng
const ordersEl = document.getElementById('orders');
let ordersCache = [];

async function loadMyOrders() {
  if (!ordersEl) return;
  try {
    const orders = await api('/orders/me');
    ordersCache = orders;
    renderOrders(orders);
  } catch (err) { alert(err.message); }
}
window.loadMyOrders = loadMyOrders;

function statusText(st) {
  return st === 'pending' ? 'Đang chuẩn bị' :
         st === 'confirmed' ? 'Đang giao' :
         st === 'delivered' ? 'Đã giao' :
         st === 'cancelled' ? 'Đã huỷ' : st;
}

function renderOrders(orders) {
  if (!ordersEl) return;
  ordersEl.innerHTML = orders.map(o => `
    <li class="card">
      <b>Đơn ${o.code || o._id}</b> — <span class="badge">${statusText(o.status)}</span><br/>
      <i>Thời gian: ${new Date(o.createdAt).toLocaleString()}</i><br/>
      <b>Tổng: ${o.total}đ</b>
      <ul>
        ${o.items.map(i => `<li>${i.quantity} x ${i.dish?.name || 'Món'} (${i.price}đ)</li>`).join('')}
      </ul>
    </li>
  `).join('');
}

function filterOrders() {
  const statusSel = document.getElementById('filterStatus');
  const searchOrder = document.getElementById('searchOrder');
  let filtered = ordersCache;
  if (statusSel?.value) filtered = filtered.filter(o => o.status === statusSel.value);
  const q = searchOrder?.value?.trim();
  if (q) filtered = filtered.filter(o =>
    (o.code || '').toLowerCase().includes(q.toLowerCase()) ||
    o.items.some(i => (i.dish?.name || '').toLowerCase().includes(q.toLowerCase()))
  );
  renderOrders(filtered);
}
window.filterOrders = filterOrders;

// Admin
const addDishForm = document.getElementById('addDishForm');
const adminDishesEl = document.getElementById('adminDishes');

if (addDishForm) {
  addDishForm.addEventListener('submit', async (e) => {
    e.preventDefault();
    const body = Object.fromEntries(new FormData(addDishForm).entries());
    body.price = Number(body.price);
    try {
      await api('/dishes', { method: 'POST', body: JSON.stringify(body) });
      alert('Thêm món thành công');
      addDishForm.reset();
      loadAdminDishes();
    } catch (err) { alert(err.message); }
  });
}

async function loadAdminDishes() {
  if (!adminDishesEl) return;
  try {
    const dishes = await api('/dishes');
    adminDishesEl.innerHTML = dishes.map(d => `
      <li class="card">
        <b>${d.name}</b><br/>
        ${d.price}đ · ★ ${d.rating ?? '4.0'} · <span class="badge">${d.category}</span><br/>
        <textarea id="desc-${d._id}" placeholder="Mô tả">${d.description || ''}</textarea>
        <button onclick="updateDish('${d._id}')">Cập nhật</button>
        <button onclick="deleteDish('${d._id}')">Xóa</button>
      </li>
    `).join('');
  } catch (err) { alert(err.message); }
}
window.loadAdminDishes = loadAdminDishes;

async function updateDish(id) {
  try {
    const desc = document.getElementById(`desc-${id}`).value;
    await api(`/dishes/${id}`, { method: 'PUT', body: JSON.stringify({ description: desc }) });
    alert('Cập nhật thành công');
    loadAdminDishes();
  } catch (err) { alert(err.message); }
}
window.updateDish = updateDish;

async function deleteDish(id) {
  try {
    await api(`/dishes/${id}`, { method: 'DELETE' });
    alert('Đã xóa món');
    loadAdminDishes();
  } catch (err) { alert(err.message); }
}
window.deleteDish = deleteDish;

// Khởi tạo theo trang
document.addEventListener('DOMContentLoaded', () => {
  if (adminDishesEl) loadAdminDishes();
  if (ordersEl && !dishesEl) loadMyOrders();
});
