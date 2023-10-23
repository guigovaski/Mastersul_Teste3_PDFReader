import { ToastContainer, toast } from 'react-toastify';
import styles from  './App.module.css'
import { uploadPdf } from './services/uploadPdf';
import { readPdfs } from './services/readPdfs';
import { Loading } from './components/Loading';
import { useState } from 'react';
import { Grid } from './components/Grid';

import 'react-toastify/dist/ReactToastify.css';

function App() {
  const [isLoading, setIsLoading] = useState(false);
  const [pdfsData, setPdfsData] = useState([]);

  const handleSubmit = async (e) => {
    setIsLoading(true);
    try {
      e.preventDefault();      
      await uploadPdf(e.target[0].files[0]);
      toast.success("PDF carregado com sucesso!", {
        autoClose: 2000,
      });
    } catch (err) {
      toast.error("Falha ao tentar carregar PDF. Tente novamente", {
        autoClose: 2000,
      });
    } finally {
      setIsLoading(false);
    }
  }

  const handleShowButtonClick = async () => {
    setIsLoading(true);
    try {
      const data = await readPdfs();
      setPdfsData(data);
    } catch (err) {
      toast.error("Falha ao tentar ler o(s) PDF(s). Tente novamente", {
        autoClose: 2000,
      });
    } finally {
      setIsLoading(false);
    }
  }

  return (
    <>
      <div className={styles.container}>      
        <h1 className={styles.title}>PDF Text Extractor</h1>
        <p className={styles.subtitle}>Upload a PDF file and extract the text from it.</p>      
        <form onSubmit={handleSubmit} className={styles.fileForm}>
          <input type="file" className={styles.inputFile} />
          <input type="submit" value="Enviar" className={styles.submitButton}  />
          <hr style={{ width: "100%" }} />
        </form>
        <p className={styles.pdfExtractTitle}>Extrair texto dos PDFs carregados</p>
        <button onClick={handleShowButtonClick} className={styles.showButton}>Extrair texto</button>
        {isLoading && (
          <Loading />
        )}
        {!isLoading && pdfsData.length > 0 && (
          <Grid data={pdfsData} />
        )}
      </div>
      <ToastContainer />
    </>
  )
}

export default App
