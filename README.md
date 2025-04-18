**DownloadOrganizer**

Um Serviço em segundo plano para organizar automaticamente a pasta de Downloads do Windows, movendo arquivos para subpastas baseadas em sua extensão.

---

## ✨ Funcionalidades

- Executa diariamente (intervalo de 24 horas) a organização de arquivos na pasta de Downloads.
- Agrupa os arquivos em subpastas dentro de `Organized/<extensão>` (por exemplo, `Organized/pdf`, `Organized/jpg`).
- Mantém um contador interno de quantos arquivos foram movidos a cada execução.
- Gera um arquivo de log em `C:\DownloadOrganizerLog.txt` registrando a data/hora e o resultado da organização.
- Permite configuração opcional da pasta de Downloads via **appsettings.json** (`CustomConfig:DownloadFolderPath`).

## 🛠️ Pré-requisitos

- .NET 9.0 ou superior
- Windows (para serviços em segundo plano e pastas do sistema)

## 🚀 Instalação

1. Clone o repositório:
   ```bash
   git clone https://seu-repo.git
   cd DownloadOrganizer
   ```
2. Abra o projeto em sua IDE favorita (Visual Studio, Rider, VS Code).
3. Restaure os pacotes:
   ```bash
   dotnet restore
   ```
4. Compile:
   ```bash
   dotnet build --configuration Release
   ```

## ⚙️ Configuração

O `Worker` determina a pasta de Downloads seguindo esta ordem:

1. **appsettings.json** (chave `CustomConfig:DownloadFolderPath`).
2. Caso não exista ou seja inválida, usa `%USERPROFILE%\Downloads`.

```json
{
  "CustomConfig": {
    "DownloadFolderPath": "C:\\Users\\SeuUsuario\\Downloads"
  }
}
```

> ⚠️ Se não for configurado, será usado o caminho padrão do Windows.

## ▶️ Como usar

- **Console**: execute no terminal:
  ```bash
  dotnet run --project DownloadOrganizer.Service
  ```
- **Windows Service**: publique em release e registre o executável como serviço do Windows.

A cada execução, o serviço:
1. Valida a existência da pasta de Downloads.
2. Percorre todos os arquivos presentes.
3. Move cada arquivo para `Downloads/Organized/<extensão>`.
4. Atualiza o arquivo de log (`C:\DownloadOrganizerLog.txt`).

## 📂 Estrutura do Código

- `Worker.cs` – lógica principal de execução e agendamento.
- `OrganizeFiles()` – percorre e move arquivos.
- `FileMovingProcess()` – cria diretórios por extensão e realiza o movimento.
- `WriteLogFileAsync()` – grava mensagens de execução no log.
- `GetPcDownloadsFolder()` – busca e valida o caminho da pasta de Downloads.

## 📝 Exemplo de Log

```
2025-04-17 20:00:01: 15 Files organized successfully.
2025-04-18 20:00:01: 8 Files organized successfully.
```

## 🤝 Contribuindo

1. Fork no repositório
2. Crie uma branch com sua feature (`git checkout -b feature/nova-feature`)
3. Faça commit das mudanças (`git commit -m 'Adiciona nova feature'`)
4. Envie para a branch principal (`git push origin feature/nova-feature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto está licenciado sob a [MIT License](LICENSE).

---

#### ❤ Desenvolvido por Alt0b11

Mantenha sua pasta de Downloads sempre limpa e organizada sem esforço!
