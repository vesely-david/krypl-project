import React, { useState } from 'react';
import { Button, Message, Modal, Form, Icon, Transition} from 'semantic-ui-react'

const LoginModal = ({loginAction}) => {
  const [modalOpened, setModalOpen] = useState(false);
  const [loginFetching, setLoginFetching] = useState(false);
  const [credentials, setCredentials] = useState({username: 'kirchjan', password: 'kirchjan!Hesl0'})
  const [loginError, setLoginError] = useState(false)

  const login = async () => {
    setLoginFetching(true);
    setLoginFetching(true);
    const result = await loginAction(credentials);
    setLoginFetching(false);
    debugger;
    if(result) setModalOpen(false); 
    else setLoginError(true);
  }

  return(
    <Modal
      trigger={
        <Button
          disabled={modalOpened}
          onClick={() => setModalOpen(true)}
        >
          <Icon name='user circle' />
          Login
        </Button>
      } 
      closeIcon 
      onClose={() => setModalOpen(false)} 
      size='small'
      open={modalOpened}
      >
      <Modal.Content>
        <div>
          <Form widths='equal' loading={loginFetching}>
              <Form.Input
                error={loginError}
                label='Username'
                value={credentials.username}
                name='username'
                onChange={(e, { name, value }) => {
                  setCredentials({...credentials,[name]: value});
                  setLoginError(false);
                }}
              />
              <Form.Input
                error={loginError}
                type='password'
                label='Password'
                value={credentials.password}
                name='password'
                onChange={(e, { name, value }) => {
                  setCredentials({...credentials, [name]: value});
                  setLoginError(false);
                }}
              />                
          </Form>
          <Transition visible={loginError} animation='scale' duration={500}>
            <Message negative> 
              <Message.Header>Username & password don't match</Message.Header>
            </Message>
          </Transition>
        </div>
      </Modal.Content>
      <Modal.Actions>
        <Button
          onClick={login}
          loading={loginFetching}
        >
          Login
        </Button>
      </Modal.Actions>
    </Modal>
  )
}

export default LoginModal;