import React, { useState } from 'react';
import { Button, Message, Modal, Form, Icon, Transition} from 'semantic-ui-react'

const RegisterModal = ({registerAction}) => {
  const [modalOpened, setModalOpen] = useState(false);
  const [registerFetching, setRegisterFetching] = useState(false);
  const [credentials, setCredentials] = useState({username: '', email: '', password: ''})
  const [registerError, setRegisterError] = useState(null)

  const register = async () => {
    setRegisterFetching(true);
    const result = await registerAction(credentials);
    setRegisterFetching(false);
    setRegisterError(result !== undefined)
  }

  return(
    <Modal
      trigger={
        <Button
          disabled={modalOpened}
          onClick={() => setModalOpen(true)}
        >
          <Icon name='add user' />
          Register
        </Button>
      } 
      closeIcon
      closeOnDimmerClick={false}
      onClose={() => setModalOpen(false)} 
      size='small'
      open={modalOpened}
      >
      <Modal.Content>
        <div>
          <Form widths='equal' loading={registerFetching}>
              <Form.Input
                required
                error={registerError === false}
                label='Username'
                value={credentials.username}
                name='username'
                onChange={(e, { name, value }) => {
                  setCredentials({...credentials,[name]: value});
                  setRegisterError(null);
                }}
              />
              <Form.Input
                required
                error={registerError === false}
                type='email'
                label='Email'
                value={credentials.email}
                name='email'
                onChange={(e, { name, value }) => {
                  setCredentials({...credentials, [name]: value});
                  setRegisterError(null);
                }}
              />                   
              <Form.Input
                required
                error={registerError === false}
                type='password'
                label='Password'
                value={credentials.password}
                name='password'
                onChange={(e, { name, value }) => {
                  setCredentials({...credentials, [name]: value});
                  setRegisterError(null);
                }}
              />                
          </Form>
          <Transition visible={registerError !== null} animation='scale' duration={500}>
            <Message negative={registerError === false} positive={registerError !== false}> 
              <Message.Header>
                {registerError === false ? 'Error during registration' : 'You can now log in'}
              </Message.Header>
            </Message>
          </Transition>
        </div>
      </Modal.Content>
      <Modal.Actions>
        <Button
          onClick={register}
          loading={registerFetching}
        >
          Register
        </Button>
      </Modal.Actions>
    </Modal>
  )
}

export default RegisterModal;
