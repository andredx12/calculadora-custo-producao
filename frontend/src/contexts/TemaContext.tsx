import { createContext, useContext, useEffect, useState, type ReactNode } from "react";

type Tema = "claro" | "escuro";

interface TemaContextType {
  tema: Tema;
  alternarTema: () => void;
}

const TemaContext = createContext<TemaContextType | undefined>(undefined);

const CHAVE_ARMAZENAMENTO = "calculadora-custo:tema";

function obterTemaInicial(): Tema {
  const salvo = localStorage.getItem(CHAVE_ARMAZENAMENTO);
  if (salvo === "claro" || salvo === "escuro") return salvo;

  const prefereEscuro = window.matchMedia("(prefers-color-scheme: dark)").matches;
  return prefereEscuro ? "escuro" : "claro";
}

export function TemaProvider({ children }: { children: ReactNode }) {
  const [tema, setTema] = useState<Tema>(obterTemaInicial);

  useEffect(() => {
    const raiz = document.documentElement;
    if (tema === "escuro") {
      raiz.classList.add("dark");
    } else {
      raiz.classList.remove("dark");
    }
    localStorage.setItem(CHAVE_ARMAZENAMENTO, tema);
  }, [tema]);

  function alternarTema() {
    setTema((atual) => (atual === "claro" ? "escuro" : "claro"));
  }

  return (
    <TemaContext.Provider value={{ tema, alternarTema }}>
      {children}
    </TemaContext.Provider>
  );
}

export function useTema() {
  const contexto = useContext(TemaContext);
  if (!contexto) {
    throw new Error("useTema deve ser usado dentro de um TemaProvider.");
  }
  return contexto;
}
