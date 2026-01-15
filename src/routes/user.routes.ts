// Importa o Router do Express.
// O Router permite criar conjuntos de rotas de forma organizada
// e reutilizável dentro da aplicação.
import { Router } from "express";

// Importa o middleware de autenticação JWT.
// Esse middleware é responsável por validar o token
// antes de permitir o acesso à rota.
import { authMiddleware } from "../middlewares/authMidlleware";

import { validateEmailMiddleware } from "../middlewares/validateEmailMiddleware";


// Cria uma nova instância do Router.
const router = Router();


const userController = new UserController();

router.post("/users", validateEmailMiddleware, userController.create);
// Rota protegida que retorna os dados do usuário autenticado.
// Método: GET
// Endpoint final: /user/me
// Requer header:
// Authorization: Bearer <token>
//
// Fluxo:
// 1. O middleware authMiddleware valida o token JWT
// 2. Se o token for válido, os dados do usuário são anexados à requisição
// 3. A rota retorna as informações do usuário autenticado
router.get("/me", authMiddleware, (req, res) => {
  return res.json({
    message: "Acesso permitido",
    user: req.user,
  });
});

// Exporta o router para ser utilizado no arquivo principal do servidor.
export default router;
