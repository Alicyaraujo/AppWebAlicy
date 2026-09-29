using appWebAlicy.Configs;
using AppWebAlicy.Model;
using MySql.Data.MySqlClient;

namespace appWebAlicy.DAO;

public class ProcessoDAO
{
    private readonly Conexao _conexao;

    public ProcessoDAO(Conexao conexao)
    {
        _conexao = conexao;
    }

    public List<Processo> Listar()
    {
        var processos = new List<Processo>();

        using var con = _conexao.GetConnection();
        const string sql = @"
            SELECT id, numero_pro, data_pro, interessado_pro,
                   assunto_pro, descricao_pro, situacao_pro
            FROM processos
            ORDER BY data_pro DESC, id DESC";

        using var comando = new MySqlCommand(sql, con);
        using var reader = comando.ExecuteReader();

        while (reader.Read())
        {
            processos.Add(new Processo
            {
                Id = reader.GetInt32("id"),
                Numero = reader.IsDBNull(reader.GetOrdinal("numero_pro"))
                    ? string.Empty
                    : reader.GetString("numero_pro"),
                Data = reader.IsDBNull(reader.GetOrdinal("data_pro"))
                    ? null
                    : DateOnly.FromDateTime(reader.GetDateTime("data_pro")),
                Interessado = reader.IsDBNull(reader.GetOrdinal("interessado_pro"))
                    ? string.Empty
                    : reader.GetString("interessado_pro"),
                Assunto = reader.IsDBNull(reader.GetOrdinal("assunto_pro"))
                    ? string.Empty
                    : reader.GetString("assunto_pro"),
                Descricao = reader.IsDBNull(reader.GetOrdinal("descricao_pro"))
                    ? string.Empty
                    : reader.GetString("descricao_pro"),
                Situacao = reader.IsDBNull(reader.GetOrdinal("situacao_pro"))
                    ? "Aberto"
                    : reader.GetString("situacao_pro")
            });
        }

        return processos;
    }

    public void Inserir(Processo processo)
    {
        if (processo.Data is null)
            throw new ArgumentException("A data do processo é obrigatória.");

        using var con = _conexao.GetConnection();

        const string sql = @"
            INSERT INTO processos
                (numero_pro, data_pro, interessado_pro, assunto_pro, descricao_pro, situacao_pro)
            VALUES
                (@numero, @data, @interessado, @assunto, @descricao, @situacao)";

        using var comando = new MySqlCommand(sql, con);

        comando.Parameters.AddWithValue("@numero", processo.Numero);
        comando.Parameters.AddWithValue("@data", processo.Data.Value.ToDateTime(TimeOnly.MinValue));
        comando.Parameters.AddWithValue("@interessado", processo.Interessado);
        comando.Parameters.AddWithValue("@assunto", processo.Assunto);
        comando.Parameters.AddWithValue("@descricao", processo.Descricao);
        comando.Parameters.AddWithValue("@situacao", processo.Situacao);

        comando.ExecuteNonQuery();
    }
}
