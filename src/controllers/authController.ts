// Importa os tipos Request e Response do Express
// para tipar corretamente as funções de controller.
import { Request, Response } from "express";

// Importa o bcrypt, usado para criptografar senhas
// e comparar senhas digitadas com hashes salvos no banco.
import bcrypt from "bcrypt";

// Importa a biblioteca jsonwebtoken,
// responsável por criar e validar tokens JWT.
import jwt from "jsonwebtoken";

// Carrega automaticamente as variáveis de ambiente
// definidas no arquivo .env.
import "dotenv/config";

// Importa a instância única do Prisma Client,
// responsável pela comunicação com o banco de dados.
import prisma from "../lib/prisma";

// Chave secreta usada para assinar e validar tokens JWT.
// Deve sempre vir do .env em ambientes reais.
const JWT_SECRET: string =
  process.env.JWT_SECRET || "chave_secreta_super_segura";

// ===========================================================
// REGISTRO DE USUÁRIO
// ===========================================================
// Responsável por criar um novo usuário no sistema.
// Endpoint: POST /auth/register
export const register = async (req: Request, res: Response) => {
  // Extrai os dados do corpo da requisição
  const { name, email, password } = req.body as {
    name: string;
    email: string;
    password: string;
  };

  // Validação básica dos campos obrigatórios
  if (!name || !email || !password) {
    return res
      .status(400)
      .json({ error: "Nome, email e senha obrigatórios" });
  }

  try {
    // Criptografa a senha antes de salvar no banco
    const hashedPassword = await bcrypt.hash(password, 10);

    // Cria o usuário no banco de dados usando o Prisma
    await prisma.user.create({
      data: {
        name,
        email,
        password: hashedPassword,
      },
    });

    // Retorna sucesso sem expor dados sensíveis
    return res
      .status(201)
      .json({ message: "Usuário registrado com sucesso!" });
  } catch (error: any) {
    console.error(error);

    // Erro P2002 ocorre quando um campo único é duplicado
    // (neste caso, email já existente)
    if (error.code === "P2002") {
      return res.status(409).json({ error: "Email já está em uso" });
    }

    // Erro genérico do servidor
    return res.status(500).json({ error: "Erro ao registrar usuário" });
  }
};

// ===========================================================
// LOGIN
// ===========================================================
// Responsável por autenticar o usuário e gerar o token JWT.
// Endpoint: POST /auth/login
export const login = async (req: Request, res: Response) => {
  // Extrai email e senha do corpo da requisição
  const { email, password } = req.body as {
    email: string;
    password: string;
  };

  // Validação básica dos campos obrigatórios
  if (!email || !password) {
    return res.status(400).json({ error: "Email e senha obrigatórios" });
  }

  try {
    // Busca o usuário no banco pelo email
    const user = await prisma.user.findUnique({
      where: { email },
    });

    // Se o usuário não existir, retorna erro de autenticação
    if (!user) {
      return res.status(401).json({ error: "Credenciais inválidas" });
    }

    // Compara a senha digitada com o hash salvo no banco
    const validPassword = await bcrypt.compare(password, user.password);
    if (!validPassword) {
      return res.status(401).json({ error: "Credenciais inválidas" });
    }

    // Gera o token JWT com id e email do usuário
    const token = jwt.sign(
      { id: user.id, email: user.email },
      JWT_SECRET,
      { expiresIn: "1h" }
    );

    // Retorna o token para o frontend
    return res.json({
      message: "Login realizado com sucesso",
      token,
    });
  } catch (error) {
    console.error(error);

    // Erro genérico do servidor
    return res.status(500).json({ error: "Erro ao fazer login" });
  }
};
