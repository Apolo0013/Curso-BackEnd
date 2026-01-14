// Importa os tipos do Express usados para tipar o middleware.
// Request  -> representa a requisição HTTP
// Response -> representa a resposta HTTP
// NextFunction -> permite seguir para o próximo middleware ou rota
import { Request, Response, NextFunction } from "express";

// Importa a biblioteca responsável por criar e validar tokens JWT
import jwt from "jsonwebtoken";

// Interface que define o formato esperado dos dados
// contidos dentro do token JWT após a decodificação.
interface TokenPayload {
  id: number;      // ID do usuário no banco de dados
  email: string;   // Email do usuário autenticado
  iat: number;     // Timestamp de quando o token foi gerado
  exp: number;     // Timestamp de quando o token expira
}

// Chave secreta usada para validar o token JWT.
// Deve ser definida no arquivo .env.
// O valor padrão é apenas para desenvolvimento.
const JWT_SECRET = process.env.JWT_SECRET || "chave_secreta_super_segura";

// Middleware de autenticação JWT.
// Ele valida se o usuário está autenticado antes de permitir
// o acesso a rotas protegidas.
export function authMiddleware(
  req: Request,
  res: Response,
  next: NextFunction
) {
  // Recupera o header Authorization da requisição.
  // Espera-se o formato: "Bearer <token>"
  const authHeader = req.headers.authorization;

  // Se o header Authorization não existir, o usuário não está autenticado.
  if (!authHeader) {
    return res.status(401).json({ error: "Token não fornecido" });
  }

  // Separa o tipo do token (Bearer) do valor do token.
  // Exemplo: "Bearer abc.def.ghi"
  const [, token] = authHeader.split(" ");

  try {
    // Verifica se o token é válido e se não está expirado.
    // Se for válido, retorna os dados decodificados.
    const decoded = jwt.verify(token, JWT_SECRET) as TokenPayload;

    // Anexa os dados do usuário à requisição.
    // Isso permite que as rotas protegidas saibam
    // qual usuário está fazendo a requisição.
    req.user = {
      id: decoded.id,
      email: decoded.email,
    };

    // Permite que a requisição continue para a próxima rota ou middleware.
    return next();
  } catch {
    // Caso o token seja inválido ou esteja expirado,
    // o acesso à rota é negado.
    return res.status(401).json({ error: "Token inválido ou expirado" });
  }
}
