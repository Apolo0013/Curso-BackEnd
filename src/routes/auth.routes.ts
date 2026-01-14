// Importa o Router do Express.
// O Router permite organizar as rotas da aplicação
// em arquivos separados, facilitando a manutenção.
import { Router } from "express";

// Importa os controllers responsáveis pelas regras de negócio
// relacionadas à autenticação (registro e login).
import { register, login } from "../controllers/authController";

// Cria uma nova instância de Router.
const router = Router();

// Rota responsável pelo registro de novos usuários.
// Método: POST
// Endpoint final: /auth/register
// Espera no corpo da requisição:
// {
//   "name": string,
//   "email": string,
//   "password": string
// }
router.post("/register", register);

// Rota responsável pelo login do usuário.
// Método: POST
// Endpoint final: /auth/login
// Espera no corpo da requisição:
// {
//   "email": string,
//   "password": string
// }
// Retorna um token JWT se as credenciais forem válidas.
router.post("/login", login);

// Exporta o router para que ele possa ser utilizado
// no arquivo principal do servidor (server.ts).
export default router;
