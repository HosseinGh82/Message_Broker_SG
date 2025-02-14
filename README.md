# Simple Message Broker in C#

## 📌 Overview
This project is a lightweight **Message Broker** implemented in **C#** that facilitates communication between producers and consumers. It ensures reliable message delivery, maintains message order, and supports persistent storage.

## 🚀 Features
- **Web API-based Message Broker** for handling message transmission.
- **Threading Support** for concurrent message processing.
- **Message Persistence** using file-based storage.
- **Dynamic Plugin System** for loading producers and consumers.
- **Retry Mechanism** to handle failures during message transmission.
- **Custom Logging System** to track operations and errors.

## 🏗️ How It Works
1. **Producers** send messages to the message broker via the **Send API**.
2. Messages are **queued and stored** to ensure persistence.
3. **Consumers** retrieve messages using the **Receive API**.
4. The system maintains **message order** and supports **multi-threaded processing**.
5. A **retry mechanism** ensures message delivery in case of failures.

## 🛠 Logging
Logs are stored in the Logs/ directory and categorized as:
- **INFO** - General events.
- **WARNING** - Potential issues.
- **ERROR** - Critical failures.

