# Mastersul_Teste3_PDFReader

## Notas
- Para que as aplicações funcionem corretamente, você deve clonar o repositório, entrar em cada projeto (PDFReaderClient e PDFReader) e rodar individualmente
- O projeto PDFReaderClient representa o projeto front-end feito em React.js e o projeto PDFReader é um projeto back-end feito com .NET 6

## Detalhes
### PDFReader (back-end)
- Para que o projeto funcione corretamente você deve entrar no arquivo "appsettings.json" e substituir o valor da propriedade "PathToData" para um caminho onde o arquivo .xlsx se encontra na sua máquina
- Você deve buildar a solução antes de rodar
- **ATENÇÂO** as portas que a aplicação irá usar estão configuradas no arquivo "launchSettings.json" dentro do diretório "Properties", caso você precise alterar alguma porta, lembre-se de mudar as portas de chamadas HTTP no projeto front-end (PDFReaderClient) também, para que as requisições funcionem corretamente

### PDFReaderClient (front-end)
- Para que o projeto funcione corretamente, você deve instalar as dependências do projeto, rodando o comando na raiz do projeto:
 - `npm install` caso estiver usando o NPM como seu gerenciador de pacotes ou `yarn` caso esteja usando o YARN
- Para rodar o projeto no localhost, você deve usar este comando na raiz do projeto:
  - `npm run dev` caso esteja usando o NPM ou `yarn dev` caso esteja usando o YARN
- Para ver em que porta a aplicação está rodando, você deve consultar o terminal onde rodou os comandos
