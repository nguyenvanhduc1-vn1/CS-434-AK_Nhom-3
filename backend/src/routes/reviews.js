const express = require('express');
const router = express.Router();
const { auth } = require('../middleware/auth');
const { addReview, getReviews } = require('../controller/reviews');

router.get('/:dishId/reviews', getReviews);
router.post('/:dishId/reviews', auth, addReview);

module.exports = router;
