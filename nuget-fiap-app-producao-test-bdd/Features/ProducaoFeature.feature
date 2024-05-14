@bdd
Funcionalidade: Gerenciamento da Produção de Pedidos de Lanchonete
    Como um usuário da API
    Eu quero gerenciar a produção de pedidos de uma lanchonete

@bdd
Cenário: Obter todos os pedidos já enviados para produção
    Dado que eu adicionei na esteira de produção um pedido do cliente "Guilherme Arana"
    Quando eu solicito a lista de pedidos enviados para produção
    Então eu devo receber uma lista contendo o pedido do cliente "Guilherme Arana" com seu respectivo status de produção

@bdd
Cenário: Adicionar uma nova produção de pedido
    Dado que eu o pagamento está OK pela API de Pagamento
    Quando eu adiciono um pedido na esteira de produção
    Então o pedido deve ser adicionado com sucesso com o status "Recebido"

@bdd
Cenário: Obter produção de pedido por ID
    Dado que eu adicionei na esteira de produção um pedido do cliente "Eduardo Vargas"
    Quando eu solicito o status daquela produção pelo seu ID
    Então eu devo receber o status da produção do pedido do cliente "Eduardo Vargas"

@bdd
Cenário: Atualizar o status de produção de um pedido existente
    Dado que eu adicionei na esteira de produção um pedido para o cliente "Matías Zaracho"
    Quando eu atualizo o status de produção daquele pedido para alterar o status de "Recebido" para "Em Preparação"
    E eu solicito a produção do pedido pelo seu ID
    Então eu devo receber o status da produção do pedido atualizado para "Em Preparação"

@bdd
Cenário: Excluir a produção de um pedido
    Dado que eu adicionei na esteira de produção um pedido para o cliente "Alan Franco"
    Quando eu excluo a produção do pedido do cliente "Alan Franco"
    Então a produção do pedido do cliente "Alan Franco" não deve mais existir

@bdd
Cenário: Tentativa de atualizar o status de produção de um pedido inexistente
    Quando eu tento atualizar o status de produção de um pedido com o ID inexistente "12345"
    Então eu devo receber uma mensagem de erro informando que a produção do pedido não existe

@bdd
Cenário: Tentativa de excluir a produção de um pedido inexistente
    Quando eu tento excluir a produção de um pedido com o ID inexistente "54321"
    Então eu devo receber uma mensagem de erro informando que a produção do pedido não existe