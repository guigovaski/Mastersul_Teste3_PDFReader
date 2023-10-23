export const uploadPdf = async (file) => {
  const formData = new FormData();

  formData.append("file", file);

  const response = await fetch(`${import.meta.env.VITE_API_URL}/Pdfs/upload-pdf`, {
    method: "POST",
    body: formData,    
  });

  const data = await response.json();

  return data;
}