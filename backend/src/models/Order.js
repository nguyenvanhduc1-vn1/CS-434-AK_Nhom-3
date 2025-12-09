const mongoose = require('mongoose');

const orderItemSchema = new mongoose.Schema({
  dish: { type: mongoose.Schema.Types.ObjectId, ref: 'Dish', required: true },
  price: { type: Number, required: true },
  quantity: { type: Number, default: 1 }
}, { _id: false });

const orderSchema = new mongoose.Schema({
  user: { type: mongoose.Schema.Types.ObjectId, ref: 'User', required: true },
  items: [orderItemSchema],
  total: { type: Number, default: 0 },
  status: { type: String, enum: ['pending', 'confirmed', 'delivered', 'cancelled'], default: 'pending' },
  code: { type: String } // e.g., DH002
}, { timestamps: true });

module.exports = mongoose.model('Order', orderSchema);
