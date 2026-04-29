# Enterprise-Agentic-Career-Copilot

 Scope is Build AI Interview Portfolio Agent — a real-world agentic system that helps candidates tailor resumes, generate interview prep, analyze job descriptions, and track applications. So i  built a modular multi-project .NET 8 AI-agent architecture with clean separation across Domain, Application, Infrastructure, Agents, and API layers. Its in progress
- Key features 


Multi-agent orchestration
Tool/function calling
RAG over uploaded resume and job descriptions
Structured JSON outputs
Human approval before rewriting/exporting resume
Evaluation tests for agent quality
Docker Compose setup
Unit tests + integration tests
GitHub Actions CI
Clean Architecture
Observability/logging
Security: no API keys in repo

# Enterprise-Agentic-Career-Copilot - Architecture
WPF / Blazor UI
   |
.NET 8 API
   |
Agent Orchestrator
   |
------------------------------------------------
| JD Agent | Resume Agent | Interview Agent | Tracker Agent |
------------------------------------------------
   |
Tools / Plugins
- Resume parser
- JD parser
- ATS scorer
- Question generator
- SQL repository
- PDF/DOCX exporter
- Email draft generator

