const express = require('express');
const router = express.Router();
const { auth, isAdmin } = require('../middleware/auth');
const { createDish, getDishes, getDishById, updateDish, deleteDish } = require('../controller/dishes');

router.get('/', getDishes);
router.get('/:id', getDishById);

router.post('/', auth, isAdmin, createDish);
router.put('/:id', auth, isAdmin, updateDish);
router.delete('/:id', auth, isAdmin, deleteDish);

module.exports = router;

