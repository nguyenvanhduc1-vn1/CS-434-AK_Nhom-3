const Review = require('../models/Review');
const Dish = require('../models/Dish');

exports.addReview = async (req, res) => {
  try {
    const { rating, comment } = req.body;
    const dishId = req.params.dishId;
    const userId = req.user.id;

    const review = await Review.create({ dish: dishId, user: userId, rating, comment });

    const dish = await Dish.findById(dishId);
    dish.reviews.push(review._id);

    const reviews = await Review.find({ dish: dishId });
    const avgRating = reviews.reduce((sum, r) => sum + r.rating, 0) / (reviews.length || 1);
    dish.rating = Number(avgRating.toFixed(2));
    await dish.save();

    res.json(review);
  } catch (err) {
    res.status(400).json({ message: err.message });
  }
};

exports.getReviews = async (req, res) => {
  try {
    const dishId = req.params.dishId;
    const reviews = await Review.find({ dish: dishId }).populate('user', 'name email');
    res.json(reviews);
  } catch (err) {
    res.status(400).json({ message: err.message });
  }
};
