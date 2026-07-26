import { useEffect, useState } from "react";
import { ingredienteService } from "./services/ingredienteService";
import type { Ingrediente } from "./types/ingrediente";
import { useTema } from "./contexts/TemaContext";

function App() {
  const [ingredientes, setIngredientes] = useState<Ingrediente[]>([]);
  const [carregando, setCarregando] = useState(true);
  const [erro, setErro] = useState<string | null>(null);
  const { tema, alternarTema } = useTema();

  useEffect(() => {
    ingredienteService
      .listar()
      .then(setIngredientes)
      .catch((e) => setErro(e.message))
      .finally(() => setCarregando(false));
  }, []);

  return (
    <div className="min-h-screen bg-white dark:bg-slate-900 p-8">
      <div className="max-w-2xl mx-auto">
        <div className="flex justify-between items-center mb-6">
          <h1 className="text-3xl font-bold text-slate-900 dark:text-white">
            Teste de conexão
          </h1>
          <button
            onClick={alternarTema}
            className="px-4 py-2 rounded-lg bg-slate-200 dark:bg-slate-700 text-slate-900 dark:text-white"
          >
            Tema: {tema}
          </button>
        </div>

        {carregando && <p className="text-slate-600 dark:text-slate-300">Carregando...</p>}
        {erro && <p className="text-red-600">Erro: {erro}</p>}

        {!carregando && !erro && (
          <ul className="space-y-2">
            {ingredientes.length === 0 && (
              <p className="text-slate-600 dark:text-slate-300">Nenhum ingrediente cadastrado ainda.</p>
            )}
            {ingredientes.map((ing) => (
              <li
                key={ing.id}
                className="p-3 bg-slate-100 dark:bg-slate-800 rounded-lg text-slate-900 dark:text-white"
              >
                {ing.nome} — {ing.unidadePadrao}
              </li>
            ))}
          </ul>
        )}
      </div>
    </div>
  );
}

export default App;