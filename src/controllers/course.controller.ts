import { Request, Response } from "express";
import { CourseService } from "../services/course.service";

const courseService = new CourseService();

export class CourseController {
  // ===================================================
  // CRIAR CURSO
  // POST /courses
  // ===================================================
  async create(req: Request, res: Response) {
    const { title, description, price } = req.body;

    // Validação básica
    if (!title || !price) {
      return res.status(400).json({
        error: "Título e preço são obrigatórios",
      });
    }

    const course = await courseService.create({
      title,
      description,
      price,
    });

    return res.status(201).json(course);
  }

  // ===================================================
  // LISTAR TODOS OS CURSOS
  // GET /courses
  // ===================================================
  async index(req: Request, res: Response) {
    const courses = await courseService.findAll();
    return res.json(courses);
  }

  // ===================================================
  // BUSCAR CURSO POR ID
  // GET /courses/:id
  // ===================================================
  async show(req: Request, res: Response) {
    const id = Number(req.params.id);

    if (isNaN(id)) {
      return res.status(400).json({ error: "ID inválido" });
    }

    const course = await courseService.findById(id);

    if (!course) {
      return res.status(404).json({ error: "Curso não encontrado" });
    }

    return res.json(course);
  }

  // ===================================================
  // ATUALIZAR CURSO
  // PUT /courses/:id
  // ===================================================
  async update(req: Request, res: Response) {
    const id = Number(req.params.id);
    const { title, description, price } = req.body;

    if (isNaN(id)) {
      return res.status(400).json({ error: "ID inválido" });
    }

    const course = await courseService.update(id, {
      title,
      description,
      price,
    });

    return res.json(course);
  }

  // ===================================================
  // REMOVER CURSO
  // DELETE /courses/:id
  // ===================================================
  async delete(req: Request, res: Response) {
    const id = Number(req.params.id);

    if (isNaN(id)) {
      return res.status(400).json({ error: "ID inválido" });
    }

    await courseService.delete(id);

    return res.status(204).send();
  }
}
