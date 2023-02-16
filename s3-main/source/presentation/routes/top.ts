import express from 'express';
const router = express.Router();

import TopController from '../controller/top-controller';
const topController = new TopController();

router.get('/', (...args) => topController.Get(...args));
router.post('/', (...args) => topController.Post(...args));

export default router;
