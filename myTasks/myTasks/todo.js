angular.module('todoApp', [
    'officeuifabric.core',
    'officeuifabric.components'
    ]).controller('TodoListController', function ($http) {
        var todoList = this;
        todoList.todos = [
            { text: 'learn AngularJS', done: true },
            { text: 'build an AngularJS app', done: false }];

        todoList.addTodo = function () {
            todoList.todos.push({ text: todoList.todoText, done: false });
            todoList.todoText = '';
        };

        todoList.remaining = function () {
            var count = 0;
            angular.forEach(todoList.todos, function (todo) {
                count += todo.done ? 0 : 1;
            });
            return count;
        };

        todoList.archive = function () {
            var oldTodos = todoList.todos;
            todoList.todos = [];
            angular.forEach(oldTodos, function (todo) {
                if (!todo.done) todoList.todos.push(todo);
            });
        };

        todoList.name = "Bla";
        todoList.person = {
            firstName: "Andy",
            lastName: "Ray"
        }; 
        
        var onTaskComplete = function (response) {
            todoList.getTask = response.data;
        }
        $http.get("http://localhost:57600/api/tasks/1")
            .then(onTaskComplete);
    });