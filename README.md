# 🚀 Modern Chat Application

A high-performance, real-time chat application built with **ASP.NET Core** and **React**. This project features real-time messaging, video/voice calls, AI assistance, and a modern, responsive UI.

---

## ✨ Key Features

- **💬 Real-time Messaging**: Instant message delivery using SignalR.
- **🎥 Video & Voice Calls**: Peer-to-peer communication powered by WebRTC.
- **🤖 AI Assistant**: Integrated OpenAI-powered chatbot for smart assistance.
- **📁 Multimedia Support**: Send images, files, and Giphy animations.
- **📊 Interactive Polls**: Create and vote in real-time polls within chats.
- **⏳ Self-Destructing & Scheduled Messages**: Advanced message controls.
- **🌑 Modern UI**: Sleek, responsive design with Dark Mode support and Ant Design components.
- **🔐 Secure Auth**: JWT-based authentication with Google and Facebook OAuth integration.
- **🌐 Internationalization**: Multi-language support (English, Vietnamese, etc.).

---

## 🛠️ Tech Stack

### Backend
- **Framework**: ASP.NET Core API
- **Database**: PostgreSQL with Entity Framework Core
- **Real-time**: SignalR
- **Identity**: ASP.NET Core Identity
- **AI**: OpenAI API
- **Infrastructure**: Docker & Docker Compose

### Frontend
- **Framework**: React 19
- **styling**: Tailwind CSS & Ant Design
- **State Management**: React Hooks & Context API
- **RTC**: Simple-Peer (WebRTC)
- **Internationalization**: i18next

---

## 🚀 Getting Started

### Prerequisites
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (for local development)
- [Node.js](https://nodejs.org/) (for local development)

### Running with Docker (Recommended)
1. **Clone the repository**:
   ```bash
   git clone https://github.com/phanhuy16/chat_app.git
   cd chat_app
   ```
2. **Configure Environment Variables**:
   Create a `.env` file in the root directory and provide the necessary keys (OpenAI, Google OAuth, etc.). You can use `.env` as a template.
3. **Start the application**:
   ```bash
   docker-compose up -d --build
   ```
4. **Access the App**:
   - Frontend: `http://localhost`
   - Backend API: `http://localhost:7201`
   - Swagger UI: `http://localhost:7201/swagger`

---

## 📂 Project Structure

- `ChatApp_Backend/`: Backend logic following Clean Architecture.
  - `API/`: Controllers, Hubs, and Program entry point.
  - `Core/`: Domain entities and interfaces.
  - `Infrastructure/`: Database contexts and service implementations.
- `chatapp_frontend/`: React application with Tailwind and AntD.

---

## 🛠️ Development

### Local Backend Setup
```bash
cd ChatApp_Backend
dotnet restore
dotnet run --project API
```

### Local Frontend Setup
```bash
cd chatapp_frontend
npm install
npm start
```

---

## 📄 License
This project is licensed under the MIT License.
