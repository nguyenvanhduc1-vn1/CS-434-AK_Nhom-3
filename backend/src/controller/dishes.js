const Dish = require('../models/Dish');

// Create
exports.createDish = async (req, res) => {
  try {
    const dish = new Dish(req.body);
    await dish.save();
    res.json(dish);
  } catch (err) {
    res.status(400).json({ message: err.message });
  }
};

// Read list with optional query q
exports.getDishes = async (req, res) => {
  try {
    const { q } = req.query;
    const filter = q ? { name: { $regex: q, $options: 'i' } } : {};
    const dishes = await Dish.find(filter).populate('reviews');
    res.json(dishes);
  } catch (err) {
    res.status(400).json({ message: err.message });
  }
};

// Read single
exports.getDishById = async (req, res) => {
  try {
    const dish = await Dish.findById(req.params.id).populate('reviews');
    if (!dish) return res.status(404).json({ message: 'Không tìm thấy món' });
    res.json(dish);
  } catch (err) {
    res.status(400).json({ message: err.message });
  }
};

// Update
exports.updateDish = async (req, res) => {
  try {
    const dish = await Dish.findByIdAndUpdate(req.params.id, req.body, { new: true });
    if (!dish) return res.status(404).json({ message: 'Không tìm thấy món' });
    res.json(dish);
  } catch (err) {
    res.status(400).json({ message: err.message });
  }
};

// Delete
exports.deleteDish = async (req, res) => {
  try {
    const dish = await Dish.findByIdAndDelete(req.params.id);
    if (!dish) return res.status(404).json({ message: 'Không tìm thấy món' });
    res.json({ message: 'Đã xóa món' });
  } catch (err) {
    res.status(400).json({ message: err.message });
  }
};
