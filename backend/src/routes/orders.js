const express = require('express');
const router = express.Router();
const { auth, isAdmin } = require('../middleware/auth');
const { createOrder, getMyOrders, updateOrderStatus } = require('../controller/orders');

router.post('/', auth, createOrder);
router.get('/me', auth, getMyOrders);
router.put('/:id/status', auth, isAdmin, updateOrderStatus);

module.exports = router;
