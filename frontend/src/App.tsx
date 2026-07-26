import { BrowserRouter, Routes, Route } from "react-router";
import { Layout } from "./components/Layout";
import { DashboardPage } from "./pages/DashboardPage";
import { ReceitasPage } from "./pages/ReceitasPage";
import { IngredientesPage } from "./pages/IngredientesPage";
import { ProducaoPage } from "./pages/ProducaoPage";
import { VendasPage } from "./pages/VendasPage";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route element={<Layout />}>
          <Route index element={<DashboardPage />} />
          <Route path="receitas" element={<ReceitasPage />} />
          <Route path="ingredientes" element={<IngredientesPage />} />
          <Route path="producao" element={<ProducaoPage />} />
          <Route path="vendas" element={<VendasPage />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;
