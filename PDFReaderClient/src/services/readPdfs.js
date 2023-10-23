export const readPdfs = async () => {
  const response = await fetch(`${import.meta.env.VITE_API_URL}/Pdfs/read-pdfs`, {
    method: "GET",
  });
  const data = await response.json();
  return data;
}