import prisma from "../lib/prisma";

interface CreateCourseDTO {
  title: string;
  description: string;
  price: number;
}

export class CourseService {
  async create(data: CreateCourseDTO) {
    return prisma.course.create({
      data,
    });
  }

  async findAll() {
    return prisma.course.findMany();
  }

  async findById(id: number) {
    return prisma.course.findUnique({
      where: { id },
    });
  }

  async update(id: number, data: Partial<CreateCourseDTO>) {
    return prisma.course.update({
      where: { id },
      data,
    });
  }

  async delete(id: number) {
    return prisma.course.delete({
      where: { id },
    });
  }
}
