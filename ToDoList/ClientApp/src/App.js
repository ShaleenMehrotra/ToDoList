import React from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import ToDoList from './components/ToDoList';
import DeleteTask from './components/DeleteTask';
import AddTask from './components/AddTask';

import './custom.css'

export default function App() {
    return (
        <Layout>
            <Route exact path='/' component={ToDoList} />
            <Route path='/add-task' component={AddTask} />
            <Route path='/delete-task' component={DeleteTask} />
        </Layout>
    );
}
