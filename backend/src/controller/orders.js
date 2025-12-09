const Order = require('../models/Order');
const Dish = require('../models/Dish');

function genCode(n = 3) {
  const num = Math.floor(Math.random() * Math.pow(10, n)).toString().padStart(n, '0');
  return `DH${num}`;
}

exports.createOrder = async (req, res) => {
  try {
    const userId = req.user.id;
    const { items } = req.body; // [{ dishId, quantity }]
    if (!Array.isArray(items) || items.length === 0) return res.status(400).json({ message: 'Giỏ hàng trống' });

    const fullItems = [];
    let total = 0;
    for (const it of items) {
      const dish = await Dish.findById(it.dishId);
      if (!dish) return res.status(400).json({ message: 'Món không tồn tại' });
      const price = dish.price;
      const quantity = it.quantity || 1;
      fullItems.push({ dish: dish._id, price, quantity });
      total += price * quantity;
    }

    const order = await Order.create({
      user: userId,
      items: fullItems,
      total,
      status: 'pending',
      code: genCode(3)
    });

    res.json(order);
  } catch (err) {
    res.status(400).json({ message: err.message });
  }
};

exports.getMyOrders = async (req, res) => {
  try {
    const orders = await Order.find({ user: req.user.id })
      .sort({ createdAt: -1 })
      .populate('items.dish', 'name');
    res.json(orders);
  } catch (err) {
    res.status(400).json({ message: err.message });
  }
};

exports.updateOrderStatus = async (req, res) => {
  try {
    const { status } = req.body;
    const order = await Order.findByIdAndUpdate(req.params.id, { status }, { new: true });
    if (!order) return res.status(404).json({ message: 'Không tìm thấy đơn hàng' });
    res.json(order);
  } catch (err) {
    res.status(400).json({ message: err.message });
  }
};
