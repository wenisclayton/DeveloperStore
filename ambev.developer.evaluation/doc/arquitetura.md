# Arquitetura do Projeto Ambev.Developer.Evaluation

Este documento apresenta uma visão geral da arquitetura do projeto utilizando um diagrama em Mermaid.

## Diagrama de Arquitetura

```mermaid
flowchart TD
  %% Camada de API
  subgraph API_Layer
    A[Ambev.DeveloperEvaluation.WebApi]
  end

  %% Camada de Aplicação
  subgraph Application_Layer
    B[Ambev.DeveloperEvaluation.Application]
  end

  %% Camada de Domínio
  subgraph Domain_Layer
    C[Ambev.DeveloperEvaluation.Domain]
    D[Ambev.DeveloperEvaluation.Common]
  end

  %% Infraestrutura
  subgraph Infrastructure
    E[Ambev.DeveloperEvaluation.ORM]
    F[Ambev.DeveloperEvaluation.MongoDB]
    G[Ambev.DeveloperEvaluation.Integration]
    H[Ambev.DeveloperEvaluation.RabbitSubscriber]
  end

  %% Injeção de Dependências
  subgraph IoC
    I[Ambev.DeveloperEvaluation.IoC]
  end

  %% Camada de Testes
  subgraph Tests
    J[Unit Tests]
    K[Integration Tests]
    L[Functional Tests]
  end

  %% Relações entre os componentes
  %% WebApi consome funcionalidades da Application
  A --> B
  %% WebApi utiliza validações e utilitários do Common
  A --> D
  %% Application executa regras de negócio do Domain
  B --> C
  %% Application utiliza suporte de segurança e logging
  B --> D
  %% Application invoca integrações externas
  B --> G
  %% Application utiliza IoC para configuração de dependências
  B --> I
  %% Integração emite eventos de domínio
  G --> C
  %% ORM realiza mapeamento de entidades do Domain
  E --> C
  %% MongoDB é utilizado para persistência (EventStore)
  F --> C
  %% RabbitSubscriber processa eventos via RabbitMQ
  H --> G
  
  %% Configuração do IoC
  I --> A
  I --> B
  I --> E
  I --> F
  I --> G
  I --> H
  
  %% Cobertura dos testes
  J --> C
  K --> E
  L --> A
