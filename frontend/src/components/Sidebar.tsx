import { NavLink } from "react-router";
import { LayoutDashboard, ChefHat, Carrot, Factory, ShoppingCart, Sun, Moon, Menu, X } from "lucide-react";
import { useTema } from "../contexts/TemaContext";
import { useState } from "react";

const ITENS_MENU = [
  { rota: "/", rotulo: "Dashboard", icone: LayoutDashboard },
  { rota: "/receitas", rotulo: "Receitas", icone: ChefHat },
  { rota: "/ingredientes", rotulo: "Ingredientes", icone: Carrot },
  { rota: "/producao", rotulo: "Produção", icone: Factory },
  { rota: "/vendas", rotulo: "Vendas", icone: ShoppingCart },
];

function ConteudoMenu({ aoClicarLink }: { aoClicarLink?: () => void }) {
  const { tema, alternarTema } = useTema();

  return (
    <>
      <div className="px-6 py-6">
        <h1 className="text-xl font-bold text-doce-marrom-700 dark:text-white">
          Calculadora de Custo
        </h1>
        <p className="text-sm text-doce-marrom-400 dark:text-slate-400 mt-1">
          Gestão da confeitaria
        </p>
      </div>

      <nav className="flex-1 px-3 space-y-1">
        {ITENS_MENU.map(({ rota, rotulo, icone: Icone }) => (
          <NavLink
            key={rota}
            to={rota}
            end={rota === "/"}
            onClick={aoClicarLink}
            className={({ isActive }) =>
              `flex items-center gap-3 px-3 py-2.5 rounded-lg text-sm font-medium transition-colors ${
                isActive
                  ? "bg-doce-rosa-200 text-doce-marrom-700 dark:bg-slate-700 dark:text-white"
                  : "text-doce-marrom-600 hover:bg-doce-rosa-100 dark:text-slate-300 dark:hover:bg-slate-800"
              }`
            }
          >
            <Icone size={20} />
            {rotulo}
          </NavLink>
        ))}
      </nav>

      <div className="px-3 pb-6">
        <button
          onClick={alternarTema}
          className="w-full flex items-center gap-3 px-3 py-2.5 rounded-lg text-sm font-medium text-doce-marrom-600 hover:bg-doce-rosa-100 dark:text-slate-300 dark:hover:bg-slate-800 transition-colors"
        >
          {tema === "escuro" ? <Sun size={20} /> : <Moon size={20} />}
          Tema {tema === "escuro" ? "claro" : "escuro"}
        </button>
      </div>
    </>
  );
}

export function Sidebar() {
  const [abertoMobile, setAbertoMobile] = useState(false);

  return (
    <>
      <div className="lg:hidden flex items-center justify-between px-4 py-3 bg-doce-rosa-50 dark:bg-slate-900 border-b border-doce-rosa-200 dark:border-slate-800">
        <h1 className="text-lg font-bold text-doce-marrom-700 dark:text-white">
          Calculadora de Custo
        </h1>
        <button
          onClick={() => setAbertoMobile(true)}
          className="p-2 text-doce-marrom-700 dark:text-white"
        >
          <Menu size={24} />
        </button>
      </div>

      <aside className="hidden lg:flex flex-col w-64 h-screen sticky top-0 bg-doce-rosa-50 dark:bg-slate-900 border-r border-doce-rosa-200 dark:border-slate-800">
        <ConteudoMenu />
      </aside>

      {abertoMobile && (
        <div className="lg:hidden fixed inset-0 z-50 flex">
          <div
            className="fixed inset-0 bg-black/50"
            onClick={() => setAbertoMobile(false)}
          />
          <aside className="relative flex flex-col w-64 h-full bg-doce-rosa-50 dark:bg-slate-900">
            <button
              onClick={() => setAbertoMobile(false)}
              className="absolute top-4 right-4 p-2 text-doce-marrom-700 dark:text-white"
            >
              <X size={22} />
            </button>
            <ConteudoMenu aoClicarLink={() => setAbertoMobile(false)} />
          </aside>
        </div>
      )}
    </>
  );
}
