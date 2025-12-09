const express = require('express');
const cors = require('cors');

const authRoutes = require('./routes/auth');
const dishRoutes = require('./routes/dishes');
const reviewRoutes = require('./routes/reviews');
const orderRoutes = require('./routes/orders');

const app = express();

app.use(cors());
app.use(express.json());

app.use('/api/auth', authRoutes);
app.use('/api/dishes', dishRoutes);
app.use('/api/dishes', reviewRoutes);
app.use('/api/orders', orderRoutes);

app.get('/', (req, res) => res.send('QUANLIMONAN API'));

module.exports = app;

