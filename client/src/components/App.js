import React from 'react'
import { Router } from 'react-router'
import { history } from '../helpers'
import { Provider } from 'react-redux'
import PropTypes from 'prop-types'
import './styles/App.scss'

class App extends React.Component {
  static propTypes = {
    store: PropTypes.object.isRequired,
    routes: PropTypes.object.isRequired,
  }

  shouldComponentUpdate () {
    return false
  }

  render () {
    return (
      <Provider store={this.props.store}>
        <div style={{ height: '100%' }}>
          <Router history={history} children={this.props.routes} />
        </div>
      </Provider>
    )
  }
}

export default App
