import React from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import ToDoList from './components/ToDoList'
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';

import './custom.css'

export default function App() {
    return (
        <Layout>
            <Route exact path='/' component={ToDoList} />
            <Route path='/add-task' component={Counter} />
            <Route path='/delete-task' component={FetchData} />
        </Layout>
    );
}
