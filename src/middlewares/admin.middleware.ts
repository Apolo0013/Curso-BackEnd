import { Request, Response, NextFunction } from "express";
import { Role } from "@prisma/client";

export function adminMiddleware(
  req: Request,
  res: Response,
  next: NextFunction
) {
  if (!req.user || req.user.role !== Role.ADMIN) {
    return res.status(403).json({ error: "Acesso negado" });
  }

  return next();
}
