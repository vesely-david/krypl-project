import React from 'react'
import { connect } from 'react-redux'
import PropTypes from 'prop-types'
import { authenticationActions } from '../../../commonModules/authentication'
import { bindActionCreators } from 'redux'
// import { withRouter } from 'react-router-dom'
import { Button, Form } from 'semantic-ui-react'

class LoginContainer extends React.Component {
  constructor (props) {
    super(props)
    this.state = {
    }
  }

  onInputChange = (e) => {
    const { name, value } = e.target
    this.setState({ [name]: value })
  }

  onSubmit = (e) => {
    e.preventDefault()
    const { username, password } = this.state
    this.props.actions.login(username, password)
  }

  render () {
    const { isAuthenticated } = this.props
    return isAuthenticated ? (
      <h1>You are already logged in</h1>
    ) : (
      <div>
        <h1>Log in</h1>
        <Form loading={isAuthenticated === null} onSubmit={this.onSubmit}>
          <Form.Group widths='equal'>
            <Form.Input label='Username' name='username' onChange={this.onInputChange} width={6} />
            <Form.Input label='Password' name='password' type='password' onChange={this.onInputChange} width={6} />
          </Form.Group>
          <Button disabled={isAuthenticated === null}>Submit</Button>
        </Form>
      </div>
    )
  }
}

LoginContainer.propTypes = {
  actions: PropTypes.object,
  isAuthenticated : PropTypes.bool,
}

const mapDispatchToProps = (dispatch) => {
  return {
    actions: bindActionCreators(authenticationActions, dispatch)
  }
}

const mapStateToProps = ({ isAuthenticated }) => ({
  isAuthenticated
})

export default connect(mapStateToProps, mapDispatchToProps)(LoginContainer)
