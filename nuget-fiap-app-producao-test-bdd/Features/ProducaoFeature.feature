@bdd
Funcionalidade: Gerenciamento da Produ��o de Pedidos de Lanchonete
    Como um usu�rio da API
    Eu quero gerenciar a produ��o de pedidos de uma lanchonete

@bdd
Cen�rio: Obter todos os pedidos j� enviados para produ��o
    Dado que eu adicionei na esteira de produ��o um pedido do cliente "Guilherme Arana"
    Quando eu solicito a lista de pedidos enviados para produ��o
    Ent�o eu devo receber uma lista contendo o pedido do cliente "Guilherme Arana" com seu respectivo status de produ��o

@bdd
Cen�rio: Adicionar uma nova produ��o de pedido
    Dado que eu o pagamento est� OK pela API de Pagamento
    Quando eu adiciono um pedido na esteira de produ��o
    Ent�o o pedido deve ser adicionado com sucesso com o status "Recebido"

@bdd
Cen�rio: Obter produ��o de pedido por ID
    Dado que eu adicionei na esteira de produ��o um pedido do cliente "Eduardo Vargas"
    Quando eu solicito o status daquela produ��o pelo seu ID
    Ent�o eu devo receber o status da produ��o do pedido do cliente "Eduardo Vargas"

@bdd
Cen�rio: Atualizar o status de produ��o de um pedido existente
    Dado que eu adicionei na esteira de produ��o um pedido para o cliente "Mat�as Zaracho"
    Quando eu atualizo o status de produ��o daquele pedido para alterar o status de "Recebido" para "Em Prepara��o"
    E eu solicito a produ��o do pedido pelo seu ID
    Ent�o eu devo receber o status da produ��o do pedido atualizado para "Em Prepara��o"

@bdd
Cen�rio: Excluir a produ��o de um pedido
    Dado que eu adicionei na esteira de produ��o um pedido para o cliente "Alan Franco"
    Quando eu excluo a produ��o do pedido do cliente "Alan Franco"
    Ent�o a produ��o do pedido do cliente "Alan Franco" n�o deve mais existir

@bdd
Cen�rio: Tentativa de atualizar o status de produ��o de um pedido inexistente
    Quando eu tento atualizar o status de produ��o de um pedido com o ID inexistente "12345"
    Ent�o eu devo receber uma mensagem de erro informando que a produ��o do pedido n�o existe

@bdd
Cen�rio: Tentativa de excluir a produ��o de um pedido inexistente
    Quando eu tento excluir a produ��o de um pedido com o ID inexistente "54321"
    Ent�o eu devo receber uma mensagem de erro informando que a produ��o do pedido n�o existe