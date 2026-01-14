// Importa o middleware CORS.
// Ele permite que o frontend (em outra origem, ex: localhost:3000)
// consiga acessar a API sem bloqueio do navegador.
import cors from "cors";

// Importa o framework Express, responsável por criar o servidor HTTP
// e gerenciar rotas e middlewares.
import express from "express";

// Carrega automaticamente as variáveis de ambiente do arquivo .env
// (ex: DATABASE_URL, JWT_SECRET).
import "dotenv/config";

// Importa as rotas relacionadas à autenticação
// (login e registro de usuário).
import authRoutes from "./routes/auth.routes";

// Importa as rotas relacionadas ao usuário autenticado
// (ex: rota /user/me protegida por JWT).
import userRoutes from "./routes/user.routes";

// Cria a aplicação Express.
const app = express();



// Configuração do CORS.
// Define quais origens podem acessar a API e se cookies/headers
// de autenticação podem ser enviados.
app.use(
  cors({
    origin: "http://localhost:3000", // endereço do frontend
    credentials: true,               // permite envio de Authorization / cookies
  })
);




// Middleware que permite que a API receba e interprete JSON
// no corpo das requisições (req.body).
app.use(express.json());

// Registra as rotas de autenticação.
// Todas as rotas dentro de authRoutes terão o prefixo /auth.
app.use("/auth", authRoutes);

// Registra as rotas de usuário.
// Todas as rotas dentro de userRoutes terão o prefixo /user.
app.use("/user", userRoutes);

const PORT = process.env.PORT || 3333;

app.listen(PORT, () => {
  console.log(`Servidor rodando na porta http://localhost:${PORT}`);
});
// Instruções para rodar o servidor:
// 1. Instale as dependências: npm install
// 2. Compile o TypeScript: npx tsc
// 3. Inicie o servidor: node dist/server.js