import React, { useState } from 'react';
import { Button, Message, Modal, Transition} from 'semantic-ui-react'

const EditApiKeyModal = ({onClick}) => {
  const [modalOpened, setModalOpen] = useState(false);
  const [submitFetching, setSubmitFetching] = useState(false);
  const [submitError, setSubmitError] = useState(false)

  const submit = async () => {
    setSubmitFetching(true);
    const result = await onClick();
    setSubmitFetching(false);
    if(result) setModalOpen(false); 
    else setSubmitError(true);
  }

  return(
    <Modal
      trigger={
        <Button
          negative
          disabled={modalOpened}
          onClick={() => setModalOpen(true)}
        >
          Delete
        </Button>
      } 
      closeIcon 
      onClose={() => setModalOpen(false)} 
      size='small'
      open={modalOpened}
    >
      <Modal.Content>
        <div>
          <p>Are you sure? Action cannot be taken back</p>
          <Transition visible={submitError} animation='scale' duration={500}>
            <Message negative> 
              <Message.Header>Error during deleting API key</Message.Header>
            </Message>
          </Transition>
        </div>
      </Modal.Content>
      <Modal.Actions>
        <Button
          onClick={() => setModalOpen(false)}
          loading={submitFetching}
        >
          Cancel
        </Button>        
        <Button
          primary
          onClick={submit}
        >
          Delete
        </Button>
      </Modal.Actions>
    </Modal>
  )
}

export default EditApiKeyModal;