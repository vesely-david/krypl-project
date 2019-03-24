import React, { useState } from 'react';
import { Button, Message, Modal, Form, Transition} from 'semantic-ui-react'

const clearFormData = {apiKey: '', apiSecret: ''};

const EditApiKeyModal = ({onClick, newKey = false}) => {
  const [modalOpened, setModalOpen] = useState(false);
  const [submitFetching, setSubmitFetching] = useState(false);
  const [formData, setformData] = useState(clearFormData)
  const [submitError, setSubmitError] = useState(false)

  const submit = async () => {
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
          {newKey ? 'Add': 'Edit'}
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
              label='API key'
              value={formData.apiKey}
              name='apiKey'
              onChange={(e, { name, value }) => {
                setformData({...formData,[name]: value});
                setSubmitError(false);
              }}
            />
            <Form.Input
              required
              error={submitError}
              label='API Secret'
              value={formData.apiSecret}
              name='apiSecret'
              onChange={(e, { name, value }) => {
                setformData({...formData, [name]: value});
                setSubmitError(false);
              }}
            />                
          </Form>
          <Transition visible={submitError} animation='scale' duration={500}>
            <Message negative> 
              <Message.Header>Error during updating API key</Message.Header>
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

export default EditApiKeyModal;