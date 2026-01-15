import { Router } from "express";
import { CourseController } from "../controllers/course.controller";
import { authMiddleware } from "../middlewares/authMidlleware";
import { adminMiddleware } from "../middlewares/admin.middleware";

const router = Router();
const controller = new CourseController();

router.post("/", authMiddleware, adminMiddleware, controller.create);
router.get("/", authMiddleware, controller.findAll);
router.get("/:id", authMiddleware, controller.findById);
router.put("/:id", authMiddleware, adminMiddleware, controller.update);
router.delete("/:id", authMiddleware, adminMiddleware, controller.delete);

export default router;
