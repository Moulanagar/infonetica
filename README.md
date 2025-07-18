# Infonetica Workflow Engine

A minimal, in-memory workflow engine built using **.NET 8 (C#)** that supports:
- Creating workflow definitions (states, actions)
- Starting workflow instances
- Executing actions to transition between states
- Viewing instance status and history
- ✅ Simple HTML UI to interact with the API (no Postman needed!)

---

## 🌐 Live Demo (Hosted on Render)

🔗 "https://infonetica.onrender.com"

---
## 🚀 Features

- Define custom workflows (states & transitions)
- Start and track multiple workflow instances
- Simple UI to test all endpoints without Postman
- Fully in-memory (no DB needed)

---

## 🔧 API Endpoints

### `POST /workflow`
Create a new workflow definition.

#### Example Request Body
```json
{
  "id": "test-wf",
  "states": [
    { "id": "start", "name": "Start", "isInitial": true, "isFinal": false, "enabled": true },
    { "id": "end", "name": "End", "isInitial": false, "isFinal": true, "enabled": true }
  ],
  "actions": [
    { "id": "go", "name": "Go to End", "enabled": true, "fromStates": ["start"], "toState": "end" }
  ]
}
```

---

### `POST /workflow/{workflowId}/start`
Start a new instance of the given workflow.

---

### `POST /instance/{instanceId}/execute?actionId={actionId}`
Execute an action on a workflow instance.

---

### `GET /instance/{instanceId}`
Fetch the current state and history of a workflow instance.

---

## 💻 How to Run Locally

```bash
# Navigate to the project folder
cd WorkflowEngineProject

# Run the project
dotnet run

# Open in browser
http://localhost:5045
```

---

## 🧪 How to Test

1. Visit the Render URL or `localhost:5045`
2. Use the HTML form to:
   - Create a workflow
   - Start an instance (e.g. ID: `test-wf`)
   - Execute an action (e.g. Action ID: `go`)
   - View instance status

---

## 📦 Tech Stack
- C# / .NET 8
- Minimal API
- HTML + JavaScript (vanilla)
- Render (for hosting)

---
