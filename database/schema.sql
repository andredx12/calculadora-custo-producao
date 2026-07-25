CREATE EXTENSION IF NOT EXISTS "pgcrypto";
CREATE EXTENSION IF NOT EXISTS "pg_trgm";

CREATE TABLE ingredientes (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nome            VARCHAR(150) NOT NULL,
    unidade_padrao  VARCHAR(20)  NOT NULL,
    ativo           BOOLEAN      NOT NULL DEFAULT true,
    criado_em       TIMESTAMPTZ  NOT NULL DEFAULT now(),
    atualizado_em   TIMESTAMPTZ  NOT NULL DEFAULT now(),

    CONSTRAINT ck_ingredientes_unidade_padrao CHECK (unidade_padrao IN
        ('Unidade','Kg','G','Litro','Ml','Duzia','Pacote','Caixa','Xicara','ColherSopa','ColherCha'))
);

CREATE UNIQUE INDEX ux_ingredientes_nome ON ingredientes (LOWER(nome)) WHERE ativo = true;
CREATE INDEX ix_ingredientes_nome_busca ON ingredientes USING gin (nome gin_trgm_ops);

CREATE TABLE receitas (
    id                    UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nome                  VARCHAR(150) NOT NULL,
    descricao             VARCHAR(500),
    quantidade_produzida  NUMERIC(10,3) NOT NULL CHECK (quantidade_produzida > 0),
    unidade_produzida     VARCHAR(30) NOT NULL DEFAULT 'unidade',
    margem_lucro_padrao   NUMERIC(5,2) CHECK (margem_lucro_padrao >= 0),
    ativo                 BOOLEAN NOT NULL DEFAULT true,
    criado_em             TIMESTAMPTZ NOT NULL DEFAULT now(),
    atualizado_em         TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE INDEX ix_receitas_nome_busca ON receitas USING gin (nome gin_trgm_ops);
CREATE INDEX ix_receitas_ativo ON receitas (ativo);

CREATE TABLE receita_ingredientes (
    id                    UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    receita_id            UUID NOT NULL REFERENCES receitas(id) ON DELETE CASCADE,
    ingrediente_id        UUID REFERENCES ingredientes(id) ON DELETE SET NULL,
    nome_ingrediente      VARCHAR(150) NOT NULL,
    quantidade_comprada   NUMERIC(10,3) NOT NULL CHECK (quantidade_comprada > 0),
    unidade_compra        VARCHAR(20) NOT NULL,
    valor_pago            NUMERIC(10,2) NOT NULL CHECK (valor_pago >= 0),
    quantidade_utilizada  NUMERIC(10,3) NOT NULL CHECK (quantidade_utilizada > 0),
    unidade_utilizada     VARCHAR(20) NOT NULL,
    ordem                 INT NOT NULL DEFAULT 0,
    criado_em             TIMESTAMPTZ NOT NULL DEFAULT now(),
    atualizado_em         TIMESTAMPTZ NOT NULL DEFAULT now(),

    CONSTRAINT ck_ri_unidade_compra CHECK (unidade_compra IN
        ('Unidade','Kg','G','Litro','Ml','Duzia','Pacote','Caixa','Xicara','ColherSopa','ColherCha')),
    CONSTRAINT ck_ri_unidade_utilizada CHECK (unidade_utilizada IN
        ('Unidade','Kg','G','Litro','Ml','Duzia','Pacote','Caixa','Xicara','ColherSopa','ColherCha'))
);

CREATE INDEX ix_receita_ingredientes_receita_id ON receita_ingredientes (receita_id);
CREATE INDEX ix_receita_ingredientes_ingrediente_id ON receita_ingredientes (ingrediente_id);

CREATE OR REPLACE FUNCTION set_atualizado_em()
RETURNS TRIGGER AS $$
BEGIN
    NEW.atualizado_em = now();
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_ingredientes_atualizado_em BEFORE UPDATE ON ingredientes
    FOR EACH ROW EXECUTE FUNCTION set_atualizado_em();
CREATE TRIGGER trg_receitas_atualizado_em BEFORE UPDATE ON receitas
    FOR EACH ROW EXECUTE FUNCTION set_atualizado_em();
CREATE TRIGGER trg_receita_ingredientes_atualizado_em BEFORE UPDATE ON receita_ingredientes
    FOR EACH ROW EXECUTE FUNCTION set_atualizado_em();
