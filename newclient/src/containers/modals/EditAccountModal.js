import React, { useState } from 'react';
import { Button, Message, Modal, Form, Transition} from 'semantic-ui-react'

const clearFormData = {password: '', passwordConfirmation: ''};

const EditAccountModal = ({onClick, newKey = false}) => {
  const [modalOpened, setModalOpen] = useState(false);
  const [submitFetching, setSubmitFetching] = useState(false);
  const [formData, setformData] = useState(clearFormData)
  const [submitError, setSubmitError] = useState(false)
  const [passwordError, setPasswordError] = useState(false)

  const submit = async () => {
    if(formData.password !== formData.passwordConfirmation){
      setPasswordError(true);
      return;
    }
    setSubmitFetching(true);
    const result = await onClick(formData);
    setSubmitFetching(false);
    if(result) {
      setformData(clearFormData);
      setModalOpen(false);
    }
    else setSubmitError(true);
  }

  return(
    <Modal
      trigger={
        <Button
          disabled={modalOpened}
          primary={newKey}
          onClick={() => setModalOpen(true)}
        >
          Edit Account
        </Button>
      } 
      closeIcon 
      onClose={() => setModalOpen(false)} 
      size='small'
      open={modalOpened}
    >
      <Modal.Content>
        <div>
          <Form widths='equal' loading={submitFetching}>     
            <Form.Input
              required
              error={submitError}
              label='Password'
              value={formData.password}
              type="password"
              name='password'
              onChange={(e, { name, value }) => {
                setformData({...formData,[name]: value});
                setSubmitError(false);
              }}
            />
            <Form.Input
              required
              error={submitError}
              label='API Secret'
              value={formData.passwordConfirmation}
              name='passwordConfirmation'
              onChange={(e, { name, value }) => {
                setformData({...formData, [name]: value});
                setSubmitError(false);
              }}
            />                
          </Form>
          <Transition visible={submitError} animation='scale' duration={500}>
            <Message negative> 
              <Message.Header>Error during changing the password</Message.Header>
            </Message>
          </Transition>
        </div>
      </Modal.Content>
      <Modal.Actions>
        <Button
          onClick={submit}
          loading={submitFetching}
        >
          Submit
        </Button>
      </Modal.Actions>
    </Modal>
  )
}

export default EditAccountModal;