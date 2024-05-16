Feature: Gerenciamento da Produção de Pedidos de Lanchonete
    Como um usuário da API
    Eu quero gerenciar a produção de pedidos de uma lanchonete

@bdd
Scenario: Obter todos os pedidos já enviados para produção por cliente
    Given que eu adicionei na esteira de produção um pedido do cliente "Guilherme Arana"
    When eu solicito a lista de pedidos enviados para produção
    Then eu devo receber uma lista contendo o pedido do cliente "Guilherme Arana"

@bdd
Scenario: Adicionar uma nova produção de pedido
    Given que o pagamento está OK pela API de Pagamento
    When eu solicito o pedido da esteira de produção
    Then o pedido deve ser adicionado com sucesso com o status "Recebido"

@bdd
Scenario: Atualizar o status de produção de um pedido existente
    Given que eu adicionei na esteira de produção um pedido do cliente "Matías Zaracho"
    When eu atualizo o status de produção daquele pedido para alterar o status de Recebido para "Em Preparação"
    And eu solicito a produção do pedido pelo seu ID
    Then eu devo receber o status da produção do pedido atualizado para "Em Preparação"

@bdd
Scenario: Excluir a produção de um pedido
    Given que eu adicionei na esteira de produção um pedido do cliente "Alan Franco"
    When eu excluo a produção do pedido do cliente pelo ID da produção
    Then a produção do pedido do cliente "Alan Franco" não deve mais existir

@bdd
Scenario: Tentativa de atualizar o status de produção de um pedido inexistente
    Given eu tento atualizar o status de produção de um pedido com o ID inexistente "12345"
    Then eu devo receber uma mensagem de erro informando que a produção do pedido não existe

@bdd
Scenario: Tentativa de excluir a produção de um pedido inexistente
    Given eu tento excluir a produção de um pedido com o ID inexistente "54321"
    Then eu devo receber uma mensagem de erro informando que a produção do pedido não existe