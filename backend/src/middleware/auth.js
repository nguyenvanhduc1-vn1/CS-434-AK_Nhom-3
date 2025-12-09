const jwt = require('jsonwebtoken');
const { JWT_SECRET } = require('../config/env');

function auth(req, res, next) {
  const header = req.headers.authorization || '';
  const token = header.startsWith('Bearer ') ? header.slice(7) : null;
  if (!token) return res.status(401).json({ message: 'Thiếu token' });
  try {
    const payload = jwt.verify(token, JWT_SECRET);
    req.user = { id: payload.id, email: payload.email, role: payload.role };
    next();
  } catch {
    res.status(401).json({ message: 'Token không hợp lệ' });
  }
}

function isAdmin(req, res, next) {
  if (req.user?.role !== 'admin') return res.status(403).json({ message: 'Chỉ admin được phép' });
  next();
}

module.exports = { auth, isAdmin };
