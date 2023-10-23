import PropTypes from "prop-types";
import styles from "./styles.module.css";

export const Grid = ({ data }) => {
  console.log(data);

  return (
    <div className={styles.container}>
      <table>
        <thead>
          <tr>
            <th>Arquivo</th>
            <th>Natureza da Operação</th>
            <th>Valor Total da Nota</th>
            <th>Dados Adicionais</th>
          </tr>
        </thead>
        <tbody>
          {Array.isArray(data) && data.map((obj, index) => (
            <tr key={index}>            
              <td>{obj.filename}</td>
              <td>{obj.naturezaDaOperacao}</td>
              <td>{obj.valorTotalDaNota}</td>
              <td>
                {obj.dadosAdicionais.map((dado, index) => (
                  <p style={{ marginBottom: "8px" }} key={index}>{dado}</p>
                ))}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

Grid.propTypes = {
  data: PropTypes.arrayOf(PropTypes.object).isRequired,
}