import React, { Component } from 'react';
import AlertsContainer from './containers/AlertsContainer';
import Header from './containers/Header';
import Routes from './containers/Routes';
import Footer from './components/Footer';

class App extends Component {
  render() {
    return (
      <div className="App">
        <AlertsContainer />
        <div>
          <Header />
          <Routes />
          <Footer />
        </div>
      </div>
    );
  }
}

export default App;
