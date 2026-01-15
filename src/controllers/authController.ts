import { Request, Response } from "express";
import bcrypt from "bcrypt";
import jwt from "jsonwebtoken";
import "dotenv/config";
import prisma from "../lib/prisma";

const JWT_SECRET =
  process.env.JWT_SECRET || "chave_secreta_super_segura";

// ===========================================================
// REGISTRO
// ===========================================================
export const register = async (req: Request, res: Response) => {
  const { name, email, password } = req.body as {
    name: string;
    email: string;
    password: string;
  };

  if (!name || !email || !password) {
    return res
      .status(400)
      .json({ error: "Nome, email e senha obrigatórios" });
  }

  try {
    const hashedPassword = await bcrypt.hash(password, 10);

    await prisma.user.create({
      data: {
        name,
        email,
        password: hashedPassword,
        role: "USER", // padrão
      },
    });

    return res
      .status(201)
      .json({ message: "Usuário registrado com sucesso!" });
  } catch (error: any) {
    if (error.code === "P2002") {
      return res.status(409).json({ error: "Email já está em uso" });
    }

    return res.status(500).json({ error: "Erro ao registrar usuário" });
  }
};

// ===========================================================
// LOGIN
// ===========================================================
export const login = async (req: Request, res: Response) => {
  const { email, password } = req.body as {
    email: string;
    password: string;
  };

  if (!email || !password) {
    return res.status(400).json({ error: "Email e senha obrigatórios" });
  }

  try {
    const user = await prisma.user.findUnique({
      where: { email },
    });

    if (!user) {
      return res.status(401).json({ error: "Credenciais inválidas" });
    }

    const validPassword = await bcrypt.compare(password, user.password);
    if (!validPassword) {
      return res.status(401).json({ error: "Credenciais inválidas" });
    }

    // 🔐 TOKEN GERADO NO LUGAR CERTO
    const token = jwt.sign(
      {
        email: user.email,
        role: user.role,
      },
      JWT_SECRET,
      {
        subject: String(user.id),
        expiresIn: "1d",
      }
    );

    return res.json({
      message: "Login realizado com sucesso",
      token,
      user: {
        id: user.id,
        name: user.name,
        email: user.email,
        role: user.role,
      },
    });
  } catch (error) {
    return res.status(500).json({ error: "Erro ao fazer login" });
  }
};
