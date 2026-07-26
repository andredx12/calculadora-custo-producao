import { useEffect, useState } from "react";
import { Plus, Pencil, Trash2, Search, X, Check } from "lucide-react";
import { ingredienteService } from "../services/ingredienteService";
import type { Ingrediente } from "../types/ingrediente";
import { UNIDADES_MEDIDA } from "../types/unidadeMedida";

export function IngredientesPage() {
  const [ingredientes, setIngredientes] = useState<Ingrediente[]>([]);
  const [carregando, setCarregando] = useState(true);
  const [erro, setErro] = useState<string | null>(null);
  const [termoBusca, setTermoBusca] = useState("");

  const [formAberto, setFormAberto] = useState(false);
  const [editandoId, setEditandoId] = useState<string | null>(null);
  const [nomeForm, setNomeForm] = useState("");
  const [unidadeForm, setUnidadeForm] = useState("Unidade");
  const [salvando, setSalvando] = useState(false);
  const [erroForm, setErroForm] = useState<string | null>(null);

  async function carregar() {
    setCarregando(true);
    setErro(null);
    try {
      const dados = termoBusca.trim()
        ? await ingredienteService.buscar(termoBusca.trim())
        : await ingredienteService.listar(true);
      setIngredientes(dados);
    } catch (e) {
      setErro(e instanceof Error ? e.message : "Erro ao carregar ingredientes.");
    } finally {
      setCarregando(false);
    }
  }

  useEffect(() => {
    const timeout = setTimeout(carregar, 300);
    return () => clearTimeout(timeout);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [termoBusca]);

  function abrirNovo() {
    setEditandoId(null);
    setNomeForm("");
    setUnidadeForm("Unidade");
    setErroForm(null);
    setFormAberto(true);
  }

  function abrirEdicao(ing: Ingrediente) {
    setEditandoId(ing.id);
    setNomeForm(ing.nome);
    setUnidadeForm(ing.unidadePadrao);
    setErroForm(null);
    setFormAberto(true);
  }

  function fecharForm() {
    setFormAberto(false);
    setEditandoId(null);
  }

  async function salvar() {
    if (!nomeForm.trim()) {
      setErroForm("O nome é obrigatório.");
      return;
    }

    setSalvando(true);
    setErroForm(null);
    try {
      if (editandoId) {
        await ingredienteService.atualizar(editandoId, { nome: nomeForm.trim() });
      } else {
        await ingredienteService.criar({ nome: nomeForm.trim(), unidadePadrao: unidadeForm });
      }
      fecharForm();
      await carregar();
    } catch (e) {
      setErroForm(e instanceof Error ? e.message : "Erro ao salvar ingrediente.");
    } finally {
      setSalvando(false);
    }
  }

  async function excluir(id: string) {
    if (!confirm("Tem certeza que deseja excluir este ingrediente?")) return;

    try {
      await ingredienteService.desativar(id);
      await carregar();
    } catch (e) {
      alert(e instanceof Error ? e.message : "Erro ao excluir ingrediente.");
    }
  }

  return (
    <div className="max-w-3xl mx-auto">
      <div className="flex items-center justify-between mb-6">
        <h1 className="text-2xl font-bold text-doce-marrom-700 dark:text-white">
          Ingredientes
        </h1>
        <button
          onClick={abrirNovo}
          className="flex items-center gap-2 px-4 py-2 bg-doce-rosa-400 hover:bg-doce-rosa-500 text-white rounded-lg font-medium transition-colors"
        >
          <Plus size={18} />
          Novo ingrediente
        </button>
      </div>

      <div className="relative mb-4">
        <Search
          size={18}
          className="absolute left-3 top-1/2 -translate-y-1/2 text-doce-marrom-400 dark:text-slate-400"
        />
        <input
          type="text"
          value={termoBusca}
          onChange={(e) => setTermoBusca(e.target.value)}
          placeholder="Buscar ingrediente por nome..."
          className="w-full pl-10 pr-4 py-2.5 rounded-lg border border-doce-rosa-200 dark:border-slate-700 bg-white dark:bg-slate-800 text-doce-marrom-700 dark:text-white placeholder:text-doce-marrom-300 dark:placeholder:text-slate-500 focus:outline-none focus:ring-2 focus:ring-doce-rosa-300 dark:focus:ring-slate-600"
        />
      </div>

      {formAberto && (
        <div className="mb-6 p-4 rounded-lg border border-doce-rosa-200 dark:border-slate-700 bg-doce-rosa-50 dark:bg-slate-800">
          <h2 className="font-semibold text-doce-marrom-700 dark:text-white mb-3">
            {editandoId ? "Editar ingrediente" : "Novo ingrediente"}
          </h2>

          <div className="space-y-3">
            <div>
              <label className="block text-sm font-medium text-doce-marrom-600 dark:text-slate-300 mb-1">
                Nome
              </label>
              <input
                type="text"
                value={nomeForm}
                onChange={(e) => setNomeForm(e.target.value)}
                placeholder="Ex: Creme de leite"
                className="w-full px-3 py-2 rounded-lg border border-doce-rosa-200 dark:border-slate-600 bg-white dark:bg-slate-900 text-doce-marrom-700 dark:text-white focus:outline-none focus:ring-2 focus:ring-doce-rosa-300 dark:focus:ring-slate-600"
              />
            </div>

            {!editandoId && (
              <div>
                <label className="block text-sm font-medium text-doce-marrom-600 dark:text-slate-300 mb-1">
                  Unidade padrão
                </label>
                <select
                  value={unidadeForm}
                  onChange={(e) => setUnidadeForm(e.target.value)}
                  className="w-full px-3 py-2 rounded-lg border border-doce-rosa-200 dark:border-slate-600 bg-white dark:bg-slate-900 text-doce-marrom-700 dark:text-white focus:outline-none focus:ring-2 focus:ring-doce-rosa-300 dark:focus:ring-slate-600"
                >
                  {UNIDADES_MEDIDA.map((u) => (
                    <option key={u.valor} value={u.valor}>
                      {u.rotulo}
                    </option>
                  ))}
                </select>
              </div>
            )}

            {erroForm && <p className="text-red-600 text-sm">{erroForm}</p>}

            <div className="flex gap-2 pt-1">
              <button
                onClick={salvar}
                disabled={salvando}
                className="flex items-center gap-2 px-4 py-2 bg-doce-rosa-400 hover:bg-doce-rosa-500 disabled:opacity-60 text-white rounded-lg font-medium transition-colors"
              >
                <Check size={18} />
                {salvando ? "Salvando..." : "Salvar"}
              </button>
              <button
                onClick={fecharForm}
                className="flex items-center gap-2 px-4 py-2 bg-doce-marrom-100 dark:bg-slate-700 text-doce-marrom-700 dark:text-white rounded-lg font-medium transition-colors"
              >
                <X size={18} />
                Cancelar
              </button>
            </div>
          </div>
        </div>
      )}

      {carregando && <p className="text-doce-marrom-500 dark:text-slate-400">Carregando...</p>}
      {erro && <p className="text-red-600">{erro}</p>}

      {!carregando && !erro && (
        <div className="space-y-2">
          {ingredientes.length === 0 && (
            <p className="text-doce-marrom-500 dark:text-slate-400">
              Nenhum ingrediente encontrado.
            </p>
          )}

          {ingredientes.map((ing) => (
            <div
              key={ing.id}
              className="flex items-center justify-between p-3 rounded-lg bg-doce-rosa-50 dark:bg-slate-800 border border-doce-rosa-100 dark:border-slate-700"
            >
              <div>
                <p className="font-medium text-doce-marrom-700 dark:text-white">{ing.nome}</p>
                <p className="text-sm text-doce-marrom-400 dark:text-slate-400">
                  {ing.unidadePadrao}
                </p>
              </div>
              <div className="flex gap-2">
                <button
                  onClick={() => abrirEdicao(ing)}
                  className="p-2 text-doce-marrom-500 hover:bg-doce-rosa-200 dark:text-slate-300 dark:hover:bg-slate-700 rounded-lg transition-colors"
                  title="Editar"
                >
                  <Pencil size={18} />
                </button>
                <button
                  onClick={() => excluir(ing.id)}
                  className="p-2 text-red-500 hover:bg-red-50 dark:hover:bg-red-950 rounded-lg transition-colors"
                  title="Excluir"
                >
                  <Trash2 size={18} />
                </button>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
