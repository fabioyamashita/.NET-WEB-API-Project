Você precisa criar uma API REST de acordo com os requisitos abaixo:

Requisitos
1. O sistema deve ter um mecanismo de login usando JWT, com um endpoint que recebe { "login":"usuario", "senha":"m1nh@s3nh@"} e gera um token.
2. O JWT deve ter, as seguintes claims:
    iss - nome do desenvolvedor
    exp - 2 horas
    sub - nome da api
    aud - "ada"
    module - "Web III .net"
3. O sistema deve ter um middleware que valide se o token é correto, valido e não está expirado, antes de permitir acesso a qualquer outro endpoint. Em caso negativo retorne status 401.
4. O login e senha fornecidos devem estar em variáveis de ambiente e terem uma versão para o ambiente de desenvolvimento vinda de um arquivo de configuração no .NET. Esse arquivo não deve subir ao GIT, mas sim um arquivo de exemplo sem os valores reais. O mesmo vale para qualquer "segredo" do sistema, como a chave do JWT.
5. Os endpoints da aplicação devem usar a porta 5000 e ser:
    (POST)      http://0.0.0.0:5000/login/

    (POST)      http://0.0.0.0:5000/api_controller/query      Obs: Deve ser possível filtrar os dados informando um corpo (body), e deve ser paginado
    
    (GET)       http://0.0.0.0:5000/api_controller/           Obs: As consultas devem ser paginadas
    
    (GET)       http://0.0.0.0:5000/api_controller/{id}
    
    (POST)      http://0.0.0.0:5000/api_controller/
    
    (PUT)       http://0.0.0.0:5000/api_controller/{id}
    
    (PATCH)     http://0.0.0.0:5000/api_controller/{id}
    
    (DELETE)    http://0.0.0.0:5000/api_controller/{id}
    

6. Para inserir um registro não deve ser informado o Id.
7. Para atualizar os dados com PATCH, devem ser enviados somente os campos que serão alterados (exceto o Id)
8. Para atualizar os dados com PUT, devem ser enviados todos os campos no body (exceto o Id)
9.  A paginação e os filtros para pesquisa podem ser feita tanto no repositório quanto utilizando funções lambda na controller
10. Todas as controllers e endpoints devem definir as responses válidos e inválidos e o tipo de objeto que retorna (model)
11. Deve ser usada alguma forma de persistência. Pode-se usar o Entity Framework (in-memory), arquivo de texto, json, dicionário, etc.
12. Se preferir optar por utilizar um banco de dados "real", adicione um docker-compose em seu repositório que coloque a aplicação e o banco em execução, quando executado docker-compose up na raiz. A connection string e a senha do banco devem ser setados por arquivo de configuração nesse arquivo.
13. Faça um filter, e crie uma classe específica para gravar logs, que escreva no console sempre que os endpoints de alteração (put, patch) ou remoção (delete) forem usados, indicando o horário formatado como o datetime a seguir: 01/01/2021 13:45:00. A linha de log deve ter o seguinte formato (se a requisição for válida):
    <datetime> - Game <id> - <titulo> - <Remover|Alterar (e descrever a alteração)>

    Exemplo:

    01/01/2021 13:45:00 - Game 1 - Counter Strike - Removido
    01/01/2021 13:45:00 - Game 1 - Counter Strike - Alterado de X para Y
14. A aplicação deve ser configurada para aceitar requests apenas da url "localhost"
15. O projeto deve ser colocado em um repositório GITHUB ou equivalente, estar público, e conter um readme.md que explique em detalhes qualquer comando ou configuração necessária para fazer o projeto rodar. Por exemplo, como configurar as variáveis de ambiente, como rodar migrations (se foram usadas).
16. A entrega será a URL do repositório ou arquivo zipado.
17. Entrega até o dia 16/09/2022, às 18:30, via Class.