import { Router } from "express";
import { CourseController } from "../controllers/course.controller";
import { authMiddleware } from "../middlewares/authMidlleware";
import { adminMiddleware } from "../middlewares/admin.middleware";

const router = Router();
const courseController = new CourseController();

router.post(
  "/",
  authMiddleware,
  adminMiddleware,
  courseController.create
);

router.get(
  "/",
  authMiddleware,
  courseController.index   // ✅ era findAll
);

router.get(
  "/:id",
  authMiddleware,
  courseController.show    // ✅ era findById
);

router.put(
  "/:id",
  authMiddleware,
  adminMiddleware,
  courseController.update
);

router.delete(
  "/:id",
  authMiddleware,
  adminMiddleware,
  courseController.delete
);

export default router;
